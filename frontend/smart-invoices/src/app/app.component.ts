import { Component, signal, computed, Signal } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { BreakpointObserver } from '@angular/cdk/layout';
import { UserService } from './services/user.service';
import { UserRole, AuthUser } from './models';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatSidenavModule,
    MatListModule,
    MatDividerModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'SmartInvoices';

  mobileMenuOpen = signal(false);

  // Properties will be initialized after constructor
  currentUser!: Signal<AuthUser | null>;
  isAdmin!: Signal<boolean>;
  isMobile!: Signal<boolean>;

  constructor(
    private userService: UserService,
    private router: Router,
    private breakpointObserver: BreakpointObserver
  ) {
    // Initialize computed signals after dependency injection
    this.currentUser = this.userService.currentUser;
    this.isAdmin = computed(() => this.currentUser()?.role === UserRole.Admin);
    this.isMobile = computed(() => this.breakpointObserver.isMatched('(max-width: 768px)'));
  }

  logout(): void {
    this.userService.logout();
    this.router.navigate(['/login']);
  }

  toggleMobileMenu(): void {
    this.mobileMenuOpen.update(open => !open);
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen.set(false);
  }
}
