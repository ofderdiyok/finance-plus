'use client'

import { useEffect, useState } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { InputNumber } from 'primereact/inputnumber';
import { Dropdown } from 'primereact/dropdown';
import { useRouter } from 'next/navigation';

export default function HarcamalarimPage() {
    const [transactions, setTransactions] = useState<any[]>([]);
    const [total, setTotal] = useState(0);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(0);
    const [first, setFirst] = useState(0);
    const [rows, setRows] = useState(10);
    const [editTransaction, setEditTransaction] = useState<any | null>(null);
    const [showDialog, setShowDialog] = useState(false);
    const [form, setForm] = useState({
        amount: 0,
        description: '',
        categoryUuid: ''
    });
    const [token, setToken] = useState<string | null>(null);
    const [categories, setCategories] = useState<any[]>([]);
    const [selectedCategoryFilter, setSelectedCategoryFilter] = useState<string>('');

    const router = useRouter();
    const dropdownOptions = [
        { name: 'Tümü', uuid: '' },
        ...categories
    ];


    useEffect(() => {
        const stored = localStorage.getItem('token');
        console.log(stored);
        if (!stored) {
            router.push('/auth/login');
            return;
        }
        setToken(stored);
    }, [router]);

    useEffect(() => {
        if (!token) return;

        fetchTransactions();

        fetch('http://localhost:8080/api/category?page=1&pageSize=100&sortBy=name&sortDir=asc', {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
        .then(async (res) => {
            const text = await res.text();

            if (!res.ok) throw new Error('Kategori alınamadı');

            const data = JSON.parse(text);
            setCategories(data.items);
        })
        .catch(console.error);
    }, [token, page, rows]);

    useEffect(() => {
        if (token) {
            fetchTransactions();
        }
    }, [token, page, rows, selectedCategoryFilter]);


    const fetchTransactions = async () => {
        if (!token) return;

        setLoading(true);
        try {
            const categoryUuid = selectedCategoryFilter ? selectedCategoryFilter : null;

            const res = await fetch(`http://localhost:8080/api/transaction/me?page=${page + 1}&pageSize=${rows}&sortBy=createdAt&sortDir=desc${selectedCategoryFilter ? `&categoryUuid=${categoryUuid}` : ''}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (!res.ok) throw new Error('Veriler alınamadı');
            const data = await res.json();
            setTransactions(data.items);
            setTotal(data.totalCount);
        } catch (err) {
            console.error(err);
        }
        setLoading(false);
    };

    const saveTransaction = async () => {
        if (!token) return;

        const method = editTransaction ? 'PUT' : 'POST';
        const url = editTransaction
            ? `http://localhost:8080/api/transaction/me/${editTransaction.uuid}`
            : `http://localhost:8080/api/transaction/me`;

        try {
            const res = await fetch(url, {
                method,
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(form)
            });

            if (res.status === 401) {
                alert("Yetki hatası. Oturum süresi dolmuş olabilir.");
                localStorage.removeItem("token");
                router.push("/auth/login");
                return;
            }

            if (!res.ok) throw new Error('Kayıt işlemi başarısız.');

            setShowDialog(false);
            setForm({ amount: 0, description: '', categoryUuid: '' });
            setEditTransaction(null);
            fetchTransactions();
        } catch (err) {
            console.error(err);
        }
    };

    const deleteTransaction = async (uuid: string) => {
        if (!token) return;

        await fetch(`http://localhost:8080/api/transaction/me/${uuid}`, {
            method: 'DELETE',
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        fetchTransactions();
    };

    const openEdit = (row: any) => {
        setForm({ amount: row.amount, description: row.description, categoryUuid: row.categoryUuid });
        setEditTransaction(row);
        setShowDialog(true);
    };

    return (
        <div className="p-5">
            <h2 className="mb-4">Harcamalarım</h2>

            <div className="mb-3 flex justify-content-between align-items-center">
                <Dropdown
                    value={selectedCategoryFilter ?? ''}
                    options={dropdownOptions}
                    optionLabel="name"
                    optionValue="uuid"
                    placeholder="Kategori Filtrele"
                    onChange={(e) => {
                        console.log(e.value)
                        setSelectedCategoryFilter(e.value);
                        setPage(0);
                    }}
                    className="w-15rem"
                />

                <Button
                    label="Yeni Harcama"
                    icon="pi pi-plus"
                    onClick={() => {
                        setForm({ amount: 0, description: '', categoryUuid: '' });
                        setShowDialog(true);
                    }}
                    className="mr-2"
                />
            </div>


            <DataTable
                value={transactions}
                paginator
                rows={rows}
                totalRecords={total}
                lazy
                loading={loading}
                first={first}
                onPage={(e) => {
                    setPage(e.page ?? 0);
                    setFirst(e.first);
                    setRows(e.rows);
                }}
                responsiveLayout="scroll"
            >
                <Column field="amount" header="Tutar" body={(row) => `${row.amount.toFixed(2)} ₺`} />
                <Column field="description" header="Açıklama" />
                <Column field="createdAt" header="Tarih" body={(row) => new Date(row.createdAt).toLocaleString('tr-TR')} />
                <Column
                    header="İşlemler"
                    body={(row) => (
                        <div className="flex gap-2">
                            <Button icon="pi pi-pencil" className="p-button-sm" onClick={() => openEdit(row)} />
                            <Button icon="pi pi-trash" className="p-button-danger p-button-sm" onClick={() => deleteTransaction(row.uuid)} />
                        </div>
                    )}
                />
            </DataTable>

            <Dialog header={editTransaction ? 'Harcamayı Güncelle' : 'Yeni Harcama'} visible={showDialog} onHide={() => setShowDialog(false)}>
                <div className="p-fluid">
                    <div className="field mb-3">
                        <label htmlFor="amount">Tutar</label>
                        <InputNumber
                            id="amount"
                            value={form.amount}
                            onValueChange={(e) => setForm({ ...form, amount: e.value || 0 })}
                            mode="decimal"
                            minFractionDigits={2}
                        />
                    </div>
                    <div className="field mb-3">
                        <label htmlFor="category">Kategori</label>
                        <Dropdown
                            id="category"
                            value={form.categoryUuid}
                            options={categories}
                            optionLabel="name"
                            optionValue="uuid"
                            placeholder="Kategori Seçin"
                            onChange={(e) => setForm({ ...form, categoryUuid: e.value })}
                            className="w-full md:w-14rem"
                            panelStyle={{ maxHeight: '300px' }}
                        />
                    </div>
                    <div className="field mb-3">
                        <label htmlFor="description">Açıklama</label>
                        <InputText
                            id="description"
                            value={form.description}
                            onChange={(e) => setForm({ ...form, description: e.target.value })}
                        />
                    </div>
                    <Button label="Kaydet" icon="pi pi-check" onClick={saveTransaction} autoFocus />
                </div>
            </Dialog>
        </div>
    );
}
