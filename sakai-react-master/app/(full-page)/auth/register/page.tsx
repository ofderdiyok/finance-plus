'use client';

import { useRouter } from 'next/navigation';
import React, { useContext, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { Button } from 'primereact/button';
import { LayoutContext } from '../../../../layout/context/layoutcontext';
import { classNames } from 'primereact/utils';
import Link from 'next/link';

const RegisterPage = () => {
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [iban, setIban] = useState('TR');
    const [currency, setCurrency] = useState('TRY');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { layoutConfig } = useContext(LayoutContext);
    const router = useRouter();

    const containerClassName = classNames(
        'surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden',
        { 'p-input-filled': layoutConfig.inputStyle === 'filled' }
    );

    const handleRegister = async () => {
        setError('');

        if (iban.length !== 26) {
            setError('IBAN 26 karakter olmalıdır.');
            return;
        }

        if (password.length < 6) {
            setError('Şifre en az 6 karakter olmalıdır.');
            return;
        }

        try {
            const response = await fetch('http://localhost:8080/api/Auth/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    fullName,
                    email,
                    iban,
                    currency,
                    password,
                    userType: 1
                }),
            });

            if (!response.ok) {
                const errText = await response.text();
                setError(errText || 'Kayıt başarısız.');
                return;
            }

            const data = await response.json();
            localStorage.setItem('token', data.token); 
            router.push('/');
        } catch (err) {
            setError('Sunucu hatası. Lütfen tekrar deneyin.');
        }
    };

    return (
        <div className={containerClassName}>
            <div className="flex flex-column align-items-center justify-content-center">
                <h1>Finance Plus</h1>
                <div
                    style={{
                        borderRadius: '56px',
                        padding: '0.3rem',
                        background: 'linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)'
                    }}
                >
                    <div className="w-full surface-card py-8 px-5 sm:px-8" style={{ borderRadius: '53px' }}>
                        <div className="text-center mb-5">
                            <div className="text-900 text-3xl font-medium mb-3">Yeni Hesap Oluştur</div>
                            <span className="text-600 font-medium">Lütfen bilgilerinizi girin</span>
                        </div>

                        <div className="mb-3">
                            <label className="block text-900 text-xl font-medium mb-2">Ad Soyad</label>
                            <InputText value={fullName} onChange={(e) => setFullName(e.target.value)} className="w-full p-3 md:w-30rem" />
                        </div>

                        <div className="mb-3">
                            <label className="block text-900 text-xl font-medium mb-2">E-posta</label>
                            <InputText value={email} onChange={(e) => setEmail(e.target.value)} className="w-full p-3 md:w-30rem" />
                        </div>

                        <div className="mb-3">
                            <label className="block text-900 text-xl font-medium mb-2">IBAN</label>
                            <InputText value={iban} onChange={(e) => setIban(e.target.value)} className="w-full p-3 md:w-30rem" />
                        </div>

                        <div className="mb-3">
                            <label className="block text-900 text-xl font-medium mb-2">Para Birimi</label>
                            <InputText disabled value={currency} className="w-full p-3 md:w-30rem" />
                        </div>

                        <div className="mb-5">
                            <label className="block text-900 text-xl font-medium mb-2">Şifre</label>
                            <Password value={password} onChange={(e) => setPassword(e.target.value)} toggleMask inputClassName="w-full p-3 md:w-30rem" />
                        </div>

                        {error && <div className="text-red-500 mb-3">{error}</div>}

                        <Button label="Kayıt Ol" className="w-full p-3 text-xl" onClick={handleRegister} />

                        <div className="text-center mt-4">
                            <span>Hesabınız var mı? </span>
                            <Link href="/auth/login" className="font-medium cursor-pointer" style={{ color: 'var(--primary-color)' }}>
                                Giriş yap
                            </Link>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    );
};

export default RegisterPage;
