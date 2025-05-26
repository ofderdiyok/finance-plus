'use client'

import { useRouter } from 'next/navigation';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';

export default function ProfilPage() {
    const router = useRouter();

    const logout = () => {
        localStorage.removeItem('token');
        router.push('/auth/login');
    };

    return (
        <div className="p-5">
            <Card title="Ayarlar" className="p-4 max-w-2xl mx-auto">
                <div className="flex flex-column align-items-center gap-3">
                    <i className="pi pi-user text-4xl text-gray-600" />
                    <h2 className="text-xl font-bold">Hesap Ayarları</h2>
                    <p className="text-600">Buradan çıkış yapabilirsiniz.</p>
                    <Button
                        label="Çıkış Yap"
                        icon="pi pi-sign-out"
                        className="p-button-secondary"
                        onClick={logout}
                    />
                </div>
            </Card>
        </div>
    );
}
