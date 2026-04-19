import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseChartDirective } from 'ng2-charts';
import { LinkService } from '../../core/services/link.service';
import { LinkAnalytics } from '../../core/models/link-analytics.model';
import { Link } from '../../core/models/link.model';
import { Chart, ChartData, ChartOptions, registerables } from 'chart.js';
import { MatCardModule } from '@angular/material/card';
import { environment } from '../../../environments/environment';
Chart.register(...registerables);

@Component({
  selector: 'app-analytics',
  imports: [
    BaseChartDirective,
    MatCardModule
  ],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss',
})
export class AnalyticsComponent implements OnInit {
  private cdr = inject(ChangeDetectorRef);
  private route = inject(ActivatedRoute);
  private linkService = inject(LinkService);

  analytics = signal<LinkAnalytics | null>(null);
  link = signal<Link | null>(null);
  
  readonly shortUrlBase = environment.shortUrlBase;
  
  chartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Clicks'
    }]
  };
  chartOptions: ChartOptions = {
    maintainAspectRatio: false
  };

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.linkService.getAnalytics(id).subscribe(
      data => {
        console.log(data);
        this.analytics.set(data);
        this.chartData = {
          labels: data.clickEvents30Days.map(c => c.date),
          datasets: [{ 
            data: data.clickEvents30Days.map(c => c.count),
            label: "clicks"
           }]
        };
        this.cdr.detectChanges();
      }
    );
    this.linkService.getLink(id).subscribe(
      data => {
        this.link.set(data);
        this.cdr.detectChanges();
      }
    );
  }
}
