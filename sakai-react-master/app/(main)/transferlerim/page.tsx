'use client';
import { Dialog } from 'primereact/dialog';
import { useEffect, useState } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { useRouter } from 'next/navigation';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';

export default function TransferlerimPage() {
    const [transfers, setTransfers] = useState<any[]>([]);
    const [total, setTotal] = useState(0);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(0);
    const [first, setFirst] = useState(0);
    const [rows, setRows] = useState(10);
    const [token, setToken] = useState<string | null>(null);
    const [form, setForm] = useState({
        amount: 0,
        description: '',
        receiverUuid: '',
        transferType: 0
    });
    const [showDialog, setShowDialog] = useState(false);
    const [users, setUsers] = useState<any[]>([]);
    const [editTransfer, setEditTransfer] = useState<any | null>(null);

    const router = useRouter();

    const saveTransfer = async () => {
        if (!token) return;

        const method = editTransfer ? 'PUT' : 'POST';
        const url = editTransfer
            ? `http://localhost:8080/api/transfer/me/${editTransfer.uuid}`
            : `http://localhost:8080/api/transfer/me`;

        try {
            const res = await fetch(url, {
                method,
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(form)
            });

            if (!res.ok) throw new Error('Transfer kaydedilemedi');

            setShowDialog(false);
            setForm({ amount: 0, description: '', receiverUuid: '', transferType: 0 });
            setEditTransfer(null); 
            fetchTransfers();
        } catch (err) {
            console.error(err);
        }
    };


    useEffect(() => {
        const stored = localStorage.getItem('token');
        if (!stored) {
            router.push('/auth/login');
            return;
        }
        setToken(stored);
    }, [router]);

    useEffect(() => {
        if (!token) return;
            
        fetch("http://localhost:8080/api/User", {
        headers: {
            Authorization: `Bearer ${token}`,
        }
        })
            .then(res => res.json())
            .then(data => setUsers(data.items))
            .catch(console.error);

        fetchTransfers();
    }, [token]);

    useEffect(() => {
        if (token) fetchTransfers();
    }, [token, page, rows]);

    const fetchTransfers = async () => {
        setLoading(true);
        try {
            const res = await fetch(`http://localhost:8080/api/transfer/me?page=${page + 1}&pageSize=${rows}&sortBy=createdAt&sortDir=desc`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (!res.ok) throw new Error('Transferler alınamadı');

            const data = await res.json();
            setTransfers(data.items);
            setTotal(data.totalCount);
        } catch (err) {
            console.error(err);
        }
        setLoading(false);
    };

    const openEdit = (row: any) => {
        setForm({
            amount: row.amount,
            description: row.description,
            receiverUuid: row.receiverUuid,
            transferType: row.transferType
        });
        setEditTransfer(row); // bu yeni state olacak
        setShowDialog(true);
    };

    const deleteTransfer = async (uuid: string) => {
        if (!token) return;

        try {
            const res = await fetch(`http://localhost:8080/api/transfer/me/${uuid}`, {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (!res.ok) throw new Error('Transfer silinemedi');

            fetchTransfers();
        } catch (err) {
            console.error(err);
        }
    };


    return (
        <div className="p-5">
            <h2 className="mb-4">Transferlerim</h2>
            <div className="mb-3 text-right">
                <Button
                    label="Yeni Transfer"
                    icon="pi pi-plus"
                    onClick={() => {
                        setForm({ amount: 0, description: '', receiverUuid: '', transferType: 0 });
                        setShowDialog(true);
                    }}
                />
            </div>

            <DataTable
                value={transfers}
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
                <Column header="Alıcı" body={(row) => {
                    const u = users.find(x => x.uuid === row.receiverUuid);
                    return u?.fullName || row.receiverUuid;
                }} />
                <Column field="transferType" header="Tür" body={(row) => row.transferType === 0 ? 'Internal' : 'IBAN'} />
                <Column field="description" header="Açıklama" />
                <Column field="createdAt" header="Tarih" body={(row) => new Date(row.createdAt).toLocaleString('tr-TR')} />
                <Column header="İşlemler"
                    body={(row) => (
                        <div className="flex gap-2">
                            <Button icon="pi pi-pencil" className="p-button-sm" onClick={() => openEdit(row)} />
                            <Button icon="pi pi-trash" className="p-button-danger p-button-sm" onClick={() => deleteTransfer(row.uuid)} />
                        </div>
                    )}
                />

            </DataTable>
            <Dialog header="Yeni Transfer" visible={showDialog} onHide={() => setShowDialog(false)}>
                <div className="p-fluid">
                    <div className="field mb-3">
                        <Dropdown
                            value={form.receiverUuid}
                            options={users}
                            optionLabel="fullName"
                            optionValue="uuid"
                            onChange={(e) => setForm({ ...form, receiverUuid: e.value })}
                            filter
                            showClear
                            placeholder="Alıcı Seçin"
                        />
                    </div>
                    <div className="field mb-3">
                        <label htmlFor="amount">Tutar</label>
                        <InputNumber id="amount" value={form.amount} onValueChange={(e) => setForm({ ...form, amount: e.value || 0 })} mode="decimal" minFractionDigits={2} />
                    </div>
                    <div className="field mb-3">
                        <label htmlFor="transferType">Transfer Türü</label>
                        <Dropdown
                            value={form.transferType}
                            options={[{ label: 'Internal', value: 0 }, { label: 'IBAN', value: 1 }]}
                            onChange={(e) => setForm({ ...form, transferType: e.value })}
                            placeholder="Transfer türü seçin"
                        />
                    </div>
                    <div className="field mb-3">
                        <label htmlFor="description">Açıklama</label>
                        <InputText id="description" value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
                    </div>
                    <Button label="Kaydet" icon="pi pi-check" onClick={saveTransfer} autoFocus />
                </div>
            </Dialog>

        </div>
    );
}
