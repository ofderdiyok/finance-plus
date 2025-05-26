'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';

export default function ClientAuthGuard({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    useEffect(() => {
        const token = localStorage.getItem('token');
        const path = window.location.pathname;
        const isPublicPage =
            path.startsWith('/auth') ||
            path.startsWith('/landing');


        if (!token && !isPublicPage) {
            router.push('/auth/login');
        }
    }, []);

    return <>{children}</>;
}
