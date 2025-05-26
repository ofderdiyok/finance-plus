export interface DecodedToken {
    sub: string; 
    email: string;
    role: string;
    exp?: number;
    [key: string]: any;
}

export function getDecodedToken(): DecodedToken | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
        const payload = token.split('.')[1];
        const decoded: DecodedToken = JSON.parse(atob(payload));
        return decoded;
    } catch (error) {
        console.error('Token çözümleme hatası:', error);
        return null;
    }
}
