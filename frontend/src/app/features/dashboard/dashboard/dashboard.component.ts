import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

import { LinkFormComponent } from "../link-form/link-form.component";
import { LinkListComponent } from "../link-list/link-list.component";

import { LinkService } from '../../../core/services/link.service';

import { Dashboard } from '../../../core/models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  imports: [
    LinkFormComponent,
    LinkListComponent,
    MatCardModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  @ViewChild(LinkListComponent) linkList!: LinkListComponent;

  private linkService = inject(LinkService);
  dashboard = signal<Dashboard | null>(null);

  ngOnInit() {
    this.linkService.getDashboard().subscribe(
      data => this.dashboard.set(data)
    );
  }

  onLinkCreated() {
    this.linkList.loadLinks();
    this.linkService.getDashboard().subscribe(
      data => this.dashboard.set(data)
    );
  }
}
