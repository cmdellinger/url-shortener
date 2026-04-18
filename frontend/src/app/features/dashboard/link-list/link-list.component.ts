import { Component, inject, OnInit, signal } from '@angular/core';
import { LinkService } from '../../../core/services/link.service';
import { Link } from '../../../core/models/link.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-link-list',
  imports: [],
  templateUrl: './link-list.component.html',
  styleUrl: './link-list.component.scss',
})
export class LinkListComponent implements OnInit {
  private linkService = inject(LinkService);
  private router = inject(Router);
  links = signal<Link[] | null>(null);
  
  ngOnInit() {
    this.linkService.getLinks().subscribe(
      data => this.links.set(data)
    );
  }

  onDelete(id: number) {
    this.linkService.deleteLink(id).subscribe({
      next: () => this.linkService.getLinks().subscribe(
        data => this.links.set(data)
      )
    } );
  }

  onAnalytics(id: number) {
    this.router.navigate(['/analytics', id]);
  }

  onEdit(id: number) {

  }

  loadLinks() {
    this.linkService.getLinks().subscribe(
      data => this.links.set(data)
    );
  }
}