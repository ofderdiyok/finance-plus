import { Metadata } from 'next';
import Layout from '../../layout/layout';
import ClientAuthGuard from './ClientAuthGuard';

interface AppLayoutProps {
    children: React.ReactNode;
}

export const metadata: Metadata = {
    title: 'Finance Plus',
    description: 'Finance Plus.',
    robots: { index: false, follow: false },
    viewport: { initialScale: 1, width: 'device-width' },
    openGraph: {
        type: 'website',
        title: 'PrimeReact SAKAI-REACT',
        url: 'https://sakai.primereact.org/',
        description: 'Finance Plus',
        images: ['https://www.primefaces.org/static/social/sakai-react.png'],
        ttl: 604800
    },
    icons: {
        icon: '/favicon.ico'
    }
};

export default function AppLayout({ children }: AppLayoutProps) {
    return (
        <Layout>
            <ClientAuthGuard>{children}</ClientAuthGuard> {}
        </Layout>
    );
}
