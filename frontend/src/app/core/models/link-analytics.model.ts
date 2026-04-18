import { ClicksByDateDto } from "../dtos/links/clicks-by-date.dto";
import { ReferrerDto } from "../dtos/links/referrer.dto";

export interface LinkAnalytics {
    totalClickEvents: number;
    clickEventsToday: number;
    clickEventsWeek: number;
    clickEvents30Days: ClicksByDateDto[];
    topReferrer: ReferrerDto[];
}
