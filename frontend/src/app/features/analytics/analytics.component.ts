import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LinkService } from '../../core/services/link.service';
import { LinkAnalytics } from '../../core/models/link-analytics.model';
import { Link } from '../../core/models/link.model';

@Component({
  selector: 'app-analytics',
  imports: [],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss',
})
export class AnalyticsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private linkService = inject(LinkService);
  analytics = signal<LinkAnalytics | null>(null);
  link = signal<Link | null>(null);

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.linkService.getAnalytics(id).subscribe(
      data => this.analytics.set(data)
    );
    this.linkService.getLink(id).subscribe(
      data => this.link.set(data)
    );
  }
}
