/* eslint-disable @next/next/no-img-element */
'use client';

import { useRouter } from 'next/navigation';
import React, { useContext, useState } from 'react';
import { Checkbox } from 'primereact/checkbox';
import { Button } from 'primereact/button';
import { Password } from 'primereact/password';
import { LayoutContext } from '../../../../layout/context/layoutcontext';
import { InputText } from 'primereact/inputtext';
import { classNames } from 'primereact/utils';
import Link from 'next/link';

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [checked, setChecked] = useState(false);
    const [error, setError] = useState('');
    const { layoutConfig } = useContext(LayoutContext);
    const router = useRouter();

    const containerClassName = classNames(
        'surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden',
        { 'p-input-filled': layoutConfig.inputStyle === 'filled' }
    );

    const handleLogin = async () => {
        setError('');
        try {
            const response = await fetch('http://localhost:8080/api/Auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                const errText = await response.text();
                setError(errText || 'Giriş başarısız');
                return;
            }

            const data = await response.json();
            localStorage.setItem('token', data.token);
            
            router.replace('/');
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
                            <div className="text-900 text-3xl font-medium mb-3">Hoş geldin!</div>
                            <span className="text-600 font-medium">Devam etmek için giriş yap</span>
                        </div>

                        <div>
                            <label htmlFor="email" className="block text-900 text-xl font-medium mb-2">
                                E-posta
                            </label>
                            <InputText id="email" type="text" placeholder="E-posta adresi" value={email} onChange={(e) => setEmail(e.target.value)} className="w-full md:w-30rem mb-5" style={{ padding: '1rem' }} />

                            <label htmlFor="password" className="block text-900 font-medium text-xl mb-2">
                                Şifre
                            </label>
                            <Password inputId="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Şifrenizi girin" toggleMask className="w-full mb-5" inputClassName="w-full p-3 md:w-30rem" />

                            <div className="flex align-items-center justify-content-between mb-5 gap-5">
                            </div>

                            {error && <div className="text-red-500 mb-3">{error}</div>}

                            <Button label="Giriş Yap" className="w-full p-3 text-xl" onClick={handleLogin} />

                            <div className="text-center mt-4">
                            <span>Hesabınız yok mı? </span>
                            <Link href="/auth/register" className="font-medium cursor-pointer" style={{ color: 'var(--primary-color)' }}>
                                Kayıt Ol!
                            </Link>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;
