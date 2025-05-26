'use client'

import { useEffect, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { useRouter } from 'next/navigation';
import { getDecodedToken } from '@/utils/token';

export default function ProfilPage() {
    const [form, setForm] = useState({
        fullName: '',
        email: '',
        IBAN: '',
        currency: 'TRY'
    });
    const [token, setToken] = useState<string | null>(null);
    const [uuid, setUuid] = useState<string>('');
    const router = useRouter();

    const currencyOptions = [
        { label: 'TRY', value: 'TRY' },
        { label: 'USD', value: 'USD' },
        { label: 'EUR', value: 'EUR' },
        { label: 'GBP', value: 'GBP' }
    ];

    useEffect(() => {
        const storedToken = localStorage.getItem('token');
        if (!storedToken) {
            router.push('/auth/login');
            return;
        }

        setToken(storedToken);
        const decoded = getDecodedToken();

        if (decoded?.sub) {
            setUuid(decoded.sub);
        }
    }, []);

    useEffect(() => {
        if (uuid && token) {
            console.log('fetchUser çağrılıyor:', uuid);
            fetchUser(uuid, token);
        }
    }, [uuid, token]);

    const fetchUser = async (uuid: string, token: string) => {
        try {
            const res = await fetch(`http://localhost:8080/api/user/${uuid}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            console.log('User fetch status:', res.status);
            if (!res.ok) throw new Error('Kullanıcı verisi alınamadı');

            const data = await res.json();
            console.log('User data:', data);
            setForm({
                fullName: data.fullName || '',
                email: data.email || '',
                IBAN: data.iban || '',
                currency: data.currency || 'TRY'
            });
        } catch (err) {
            console.error('fetchUser hatası:', err);
        }
    };

    const saveProfile = async () => {
        if (!uuid || !token) return;

        try {
            const res = await fetch(`http://localhost:8080/api/user/${uuid}`, {
                method: 'PUT',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(form)
            });

            if (!res.ok) throw new Error('Güncelleme başarısız');
            alert('Profil başarıyla güncellendi.');
        } catch (err) {
            console.error('Profil güncelleme hatası:', err);
            alert('Bir hata oluştu.');
        }
    };

    const deleteAccount = async () => {
        if (!uuid || !token) return;

        const confirmDelete = confirm('Hesabınızı silmek istediğinize emin misiniz? Bu işlem geri alınamaz.');
        if (!confirmDelete) return;

        try {
            const res = await fetch(`http://localhost:8080/api/user/${uuid}`, {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (res.status === 204) {
                localStorage.removeItem('token');
                alert('Hesabınız silindi. Hoşçakal.');
                router.push('/auth/login');
            } else {
                throw new Error('Silme işlemi başarısız.');
            }
        } catch (err) {
            console.error('Hesap silme hatası:', err);
            alert('Bir hata oluştu. Lütfen tekrar deneyin.');
        }
    };


    return (
        <div className="p-5">
            <Card title="Profilim" className="p-4 max-w-2xl mx-auto">
                <div className="flex flex-column md:flex-row align-items-center gap-4 mb-4">
                    <div className="bg-gray-200 border-circle flex align-items-center justify-content-center mr-3" style={{ width: '60px', height: '60px' }}>
                        <i className="pi pi-user text-3xl text-gray-600" />
                    </div>
                    <div className="flex-1 w-full">
                        <div className="field mb-3">
                            <label>Ad Soyad</label>
                            <InputText value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} className="w-full" />
                        </div>
                        <div className="field mb-3">
                            <label>Email</label>
                            <InputText value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} className="w-full" />
                        </div>
                        <div className="field mb-3">
                            <label>IBAN</label>
                            <InputText value={form.IBAN} onChange={(e) => setForm({ ...form, IBAN: e.target.value })} className="w-full" />
                        </div>
                    </div>
                </div>
                <div className="text-right">
                    <Button label="Kaydet" icon="pi pi-save" onClick={saveProfile} />
                </div>
            </Card>

            <div className="text-right mt-4">
                <Button label="Hesabı Sil" icon="pi pi-trash" className="p-button-danger" onClick={deleteAccount} />
            </div>

        </div>
    );
}
