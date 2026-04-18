import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { Link } from '../models/link.model';
import { LinkAnalytics } from '../models/link-analytics.model';
import { Dashboard } from '../models/dashboard.model';

import { CreateLinkDto } from '../dtos/links/create-link.dto';
import { UpdateLinkDto } from '../dtos/links/update-link.dto';


@Injectable({
  providedIn: 'root',
})
export class LinkService {
  private http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  // Links
  private linksUrl = `${this.apiUrl}/api/links`;
  private linkUrl(linkId: number): string {
    return `${this.linksUrl}/${linkId}`;
  }

  getLinks(): Observable<Link[]> {
    return this.http.get<Link[]>(this.linksUrl);
  }

  getLink(id: number): Observable<Link> {
    return this.http.get<Link>(this.linkUrl(id));
  }

  createLink(createLinkDto: CreateLinkDto): Observable<Link> {
    return this.http.post<Link>(this.linksUrl, createLinkDto);
  }

  updateLink(id: number, updateLinkDto: UpdateLinkDto): Observable<void> {
    return this.http.put<void>(this.linkUrl(id), updateLinkDto);
  }

  deleteLink(id: number): Observable<void> {
    return this.http.delete<void>(this.linkUrl(id));
  }

  // Analytics
  private analyticsUrl(id: number): string {
    return `${this.apiUrl}/api/links/${id}/analytics`;
  }
  getAnalytics(id: number): Observable<LinkAnalytics> {
    return this.http.get<LinkAnalytics>(this.analyticsUrl(id));
  }

  // Dashboard
  private dashboardUrl = `${this.apiUrl}/api/dashboard`;
  getDashboard(): Observable<Dashboard> {
    return this.http.get<Dashboard>(this.dashboardUrl);
  }
}
