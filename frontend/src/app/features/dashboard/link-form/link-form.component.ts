import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAnchor, MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import {MatSnackBar} from '@angular/material/snack-bar';

import { LinkService } from '../../../core/services/link.service';

import { CreateLinkDto } from '../../../core/dtos/links/create-link.dto';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-link-form',
  imports: [
    MatAnchor,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInput,
    ReactiveFormsModule
  ],
  templateUrl: './link-form.component.html',
  styleUrl: './link-form.component.scss',
})
export class LinkFormComponent {
  @Output() linkCreated = new EventEmitter<void>();

  private linkService = inject(LinkService);
  private snackBar = inject(MatSnackBar);
  
  newLinkForm = new FormGroup({
    url: new FormControl(
      '',
      {
       validators: [
        Validators.required,
        Validators.pattern(/^https?:\/\/.+/)
       ] 
      }
    ),
    title: new FormControl()
  })

  createLink() {
    if (!this.newLinkForm.valid) return;
    const newLink: CreateLinkDto = {
      originalUrl: this.newLinkForm.value.url!,
      title: this.newLinkForm.value.title
    }
    this.linkService.createLink(newLink).subscribe({
      next: link => {
        this.newLinkForm.reset();
        this.linkCreated.emit();

        const shortUrl = `${environment.shortUrlBase}/${link.shortCode}`;
        const snackBarRef = this.snackBar.open(
          shortUrl, 'Copy', { duration: 5000 }
        );
        snackBarRef.onAction().subscribe( () => {
          navigator.clipboard.writeText(shortUrl)
        } );
      }
    });
  }
}
