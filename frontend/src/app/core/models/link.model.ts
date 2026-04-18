export interface Link {
    id: number;
    shortCode: string;
    originalUrl: string;
    title: string | null;
    createdAt: Date;
    expiresAt: Date | null;
    isActive: boolean;
    clickCount: number;
}
