import { Link } from "./link.model";

export interface Dashboard {
    totalLinks: number;
    totalClicks: number;
    clicksToday: number;
    topLinks: Link[];
}
