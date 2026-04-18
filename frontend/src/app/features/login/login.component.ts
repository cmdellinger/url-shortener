import { AfterViewInit, Component, ElementRef, inject, NgZone, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

import { GoogleLoginDto } from '../../core/dtos/auth/google-login.dto';

import { environment } from '../../../environments/environment';

declare const google: any;

@Component({
  selector: 'app-login',
  imports: [
    MatButtonModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements AfterViewInit{
  private authService = inject(AuthService);
  private router = inject(Router);
  private ngZone = inject(NgZone);

  @ViewChild('googleBtn') googleBtn!: ElementRef;

  ngAfterViewInit(): void {
    google.accounts.id.initialize({
      client_id: environment.googleClientId,
      callback: (res: any) => this.login(res)
    });

    google.accounts.id.renderButton(
      this.googleBtn.nativeElement,
      { theme: 'outline', size: 'large' }
    );
  }

  private login(response: any) {
    this.ngZone.run( () => {
      const loginDto: GoogleLoginDto = { idToken: response.credential };
      this.authService.login(loginDto).subscribe( () => {
        this.router.navigate(['/dashboard']);
      } );
    } );
  }
}
