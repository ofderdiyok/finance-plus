'use client';

import { useEffect, useState } from 'react';
import { Button } from 'primereact/button';
import { useRouter } from 'next/navigation';
import { getDecodedToken } from '@/utils/token';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';


interface User {
    uuid: string;
    fullName: string;
    email: string;
    asset: number;
    currency: string;
}

export default function Dashboard() {
    const [user, setUser] = useState<User | null>(null);
    const [rates, setRates] = useState<{ USD: number; EUR: number, GBP: number, CHF:number, JPY:number } | null>(null);
    const [metals, setMetals] = useState<{
        goldTry: number;
        silverTry: number;
        onsUSD: number;
        onsSilver: number;
        updatedAt: string;
    } | null>(null);
    const [transfers, setTransfers] = useState<any[]>([]);
    const [transactions, setTransactions] = useState<any[]>([]);
    const [users, setUsers] = useState<any[]>([]);

    const router = useRouter();

    useEffect(() => {
        const token = localStorage.getItem('token');
        const decoded = getDecodedToken();
        const uuid = decoded?.sub;

        if (!token || !uuid) return;

        fetch(`http://localhost:8080/api/User/${uuid}`, {
            headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                if (!res.ok) {
                    localStorage.removeItem('token'); 
                    return null;
                }
                return res.json();
            })
            .then((data) => setUser(data))
            .catch(() => setUser(null));

        fetch('http://localhost:8080/api/user', {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
            .then(res => res.json())
            .then(data => setUsers(data.items ?? []))
            .catch(() => setUsers([]));

        fetch('https://api.coingecko.com/api/v3/simple/price?ids=pax-gold,silver-tokenized-stock-defichain&vs_currencies=try,usd')
            .then(res => res.json())
            .then(data => {
                const onsUSD = data['pax-gold']?.usd ?? 0;
                const gramAltinTRY = (data['pax-gold']?.try ?? 0) / 31.1035;
                const onsSilver = (data['silver-tokenized-stock-defichain']?.usd ?? 0) * 31.1035;
                const silverTRY = data['silver-tokenized-stock-defichain']?.try ?? 0;
                const updatedAt = new Date().toLocaleString('tr-TR');

                setMetals({
                    onsUSD: onsUSD,
                    onsSilver: onsSilver,
                    goldTry: gramAltinTRY,
                    silverTry: silverTRY,
                    updatedAt
                });
            })

        fetch('https://api.frankfurter.app/latest?from=TRY&to=USD,EUR,GBP,CHF,JPY')
            .then((res) => res.json())
            .then((data) => setRates(data.rates))
            .catch(() => setRates(null));

        if (uuid) {
            fetch(`http://localhost:8080/api/transfer/me?page=1&pageSize=10&sortBy=createdAt&sortDir=desc`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
                .then(res => res.json())
                .then(data => setTransfers(data.items));

            fetch(`http://localhost:8080/api/transaction/me?page=1&pageSize=10&sortBy=createdAt&sortDir=desc`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
                .then(res => res.json())
                .then(data => setTransactions(data.items))
                .catch(() => setTransactions([]));
        }
    }, []);

    const getReceiverName = (uuid: string) => {
        return users.find(u => u.uuid === uuid)?.fullName || uuid;
    };

    if (!user) {
        return (
            <div className="p-5 text-center">
                <h3 className="text-900 font-bold mb-2">Oturumunuz Sona Erdi</h3>
                <p className="text-600 mb-4">Devam edebilmek için lütfen tekrar giriş yapın.</p>
                <Button
                    label="Giriş Yap"
                    icon="pi pi-sign-in"
                    className="p-button-sm"
                    onClick={() => router.push('/auth/login')}
                />
            </div>
        );
    }


    return (
        <div className="p-5">
            <div className="card mb-5 p-4">
                <div className="flex flex-column md:flex-row justify-content-between align-items-start md:align-items-center mb-3">
                    <div className="flex align-items-center">
                        <div className="bg-gray-200 border-circle flex align-items-center justify-content-center mr-3" style={{ width: '60px', height: '60px' }}>
                            <i className="pi pi-user text-3xl text-gray-600" />
                        </div>
                        <div>
                            <h2 className="text-2xl font-bold mb-1">{user.fullName}</h2>
                            <p className="text-600 mb-0">{user.email}</p>
                        </div>
                    </div>

                    <div className="text-right mt-4 md:mt-0">
                        <div className="text-500 mb-1">Toplam Varlık</div>
                        <div className="text-green-600 text-3xl font-bold">
                            {user.asset?.toLocaleString('tr-TR')} {user.currency === 'TRY' ? '₺' : user.currency}
                        </div>
                        <div className="text-500 text-sm">Kur: <strong>{user.currency}</strong></div>
                    </div>
                </div>

               <div className="text-right mt-3">
                    <Button
                        label="Profilim"
                        icon="pi pi-chevron-right"
                        className="p-button-text"
                        onClick={() => router.push('/profilim')}
                    />
                </div>
            </div>

            <div className="grid">
                {/* Döviz Kurları */}
                <div className="col-12 md:col-6">
                    {rates && (
                        <div className="card mb-4 p-4">
                            <h5 className="mb-3">Döviz Kurları</h5>
                            <div className="flex flex-column md:flex-row flex-wrap gap-4">
                                {/* USD */}
                                <div className="flex justify-content-between align-items-center w-full md:w-30rem">
                                    <div className="flex align-items-center gap-2">
                                        <img src="https://flagcdn.com/w40/us.png" width="24" alt="USD" />
                                        <span className="text-500">1 USD</span>
                                    </div>
                                    <span className="font-bold text-lg text-indigo-600">{(1 / rates.USD).toFixed(2)} ₺</span>
                                </div>

                                {/* EUR */}
                                <div className="flex justify-content-between align-items-center w-full md:w-30rem">
                                    <div className="flex align-items-center gap-2">
                                        <img src="https://flagcdn.com/w40/eu.png" width="24" alt="EUR" />
                                        <span className="text-500">1 EUR</span>
                                    </div>
                                    <span className="font-bold text-lg text-indigo-600">{(1 / rates.EUR).toFixed(2)} ₺</span>
                                </div>

                                {/* GBP */}
                                <div className="flex justify-content-between align-items-center w-full md:w-30rem">
                                    <div className="flex align-items-center gap-2">
                                        <img src="https://flagcdn.com/w40/gb.png" width="24" alt="GBP" />
                                        <span className="text-500">1 GBP</span>
                                    </div>
                                    <span className="font-bold text-lg text-indigo-600">{(1 / rates.GBP).toFixed(2)} ₺</span>
                                </div>

                                {/* CHF */}
                                <div className="flex justify-content-between align-items-center w-full md:w-30rem">
                                    <div className="flex align-items-center gap-2">
                                        <img src="https://flagcdn.com/w40/ch.png" width="24" alt="CHF" />
                                        <span className="text-500">1 CHF</span>
                                    </div>
                                    <span className="font-bold text-lg text-indigo-600">{(1 / rates.CHF).toFixed(2)} ₺</span>
                                </div>

                                {/* JPY */}
                                <div className="flex justify-content-between align-items-center w-full md:w-30rem">
                                    <div className="flex align-items-center gap-2">
                                        <img src="https://flagcdn.com/w40/jp.png" width="24" alt="JPY" />
                                        <span className="text-500">1 JPY</span>
                                    </div>
                                    <span className="font-bold text-lg text-indigo-600">{(1 / rates.JPY).toFixed(2)} ₺</span>
                                </div>
                            </div>
                        </div>
                    )}
                </div>

                {/* Kıymetli Madenler */}
                <div className="col-12 md:col-6">
                    {metals && (
                        <div className="card mb-4 p-4">
                            <h5 className="mb-3">Kıymetli Madenler</h5>
                            <div className="flex flex-column gap-3">
                                <div className="flex justify-content-between align-items-center w-full">
                                    <span className="text-500">Gram Altın</span>
                                    <span className="font-bold text-lg text-yellow-600">{metals.goldTry.toFixed(2)} ₺</span>
                                </div>
                                <div className="flex justify-content-between align-items-center w-full">
                                    <span className="text-500">Ons Altın (USD)</span>
                                    <span className="font-bold text-lg text-yellow-600">{metals.onsUSD.toFixed(2)} $</span>
                                </div>
                                <div className="flex justify-content-between align-items-center w-full">
                                    <span className="text-500">Gram Gümüş</span>
                                    <span className="font-bold text-lg text-gray-500">{metals.silverTry.toFixed(2)} ₺</span>
                                </div>
                                <div className="flex justify-content-between align-items-center w-full">
                                    <span className="text-500">ONS Gümüş (USD)</span>
                                    <span className="font-bold text-lg text-gray-500">{metals.onsSilver.toFixed(2)} $</span>
                                </div>
                                <div className="text-right mt-3 text-sm text-500">
                                    Son Güncelleme: {metals.updatedAt}
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>

           <div className="card p-4">
                <h5 className="mb-3">Son 10 Transfer</h5>

                {transfers.length > 0 ? (
                    <DataTable value={transfers} responsiveLayout="scroll">
                        <Column header="Alıcı" body={(row) => getReceiverName(row.receiverUuid)} />
                        <Column field="amount" header="Tutar" body={(row) => `${row.amount} ₺`} />
                        <Column field="description" header="Açıklama" />
                        <Column field="date" header="Tarih" body={(row) => new Date(row.date).toLocaleDateString('tr-TR')} />
                    </DataTable>
                ) : (
                    <div className="flex flex-column align-items-center justify-content-center py-4">
                        <i className="pi pi-money-bill text-3xl text-gray-400 mb-2" />
                        <p className="text-600">Henüz gerçekleşmiş bi transferin yok.</p>
                    </div>
                )}

                <div className="text-right mt-3">
                    <Button
                        label="Tüm Transferlerim"
                        icon="pi pi-chevron-right"
                        className="p-button-text"
                        onClick={() => router.push('/transferlerim')}
                    />
                </div>
            </div>


            <div className="card p-4 mt-4">
                <h5 className="mb-3">Son 10 Harcama</h5>

                {transactions.length > 0 ? (
                    <DataTable value={transactions} responsiveLayout="scroll">
                        <Column field="amount" header="Tutar" body={(row) => `${row.amount.toFixed(2)} ₺`} />
                        <Column field="description" header="Açıklama" />
                        <Column field="createdAt" header="Tarih" body={(row) => new Date(row.createdAt).toLocaleDateString('tr-TR')} />
                    </DataTable>
                ) : (
                    <div className="flex flex-column align-items-center justify-content-center py-4">
                        <i className="pi pi-wallet text-3xl text-gray-400 mb-2" />
                        <p className="text-600">Henüz harcama yapmadın.</p>
                    </div>
                )}

                <div className="text-right mt-3">
                    <Button
                        label="Tüm Harcamalarım"
                        icon="pi pi-chevron-right"
                        className="p-button-text"
                        onClick={() => router.push('/harcamalarim')}
                    />
                </div>
            </div>

        </div>
    );
}
