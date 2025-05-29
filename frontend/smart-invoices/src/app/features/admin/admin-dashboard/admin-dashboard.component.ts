import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';

import { UserService } from '../../../services/user.service';
import { InvoiceService } from '../../../services/invoice.service';
import { ChangeRequestService } from '../../../services/change-request.service';
import { RefundRequestService } from '../../../services/refund-request.service';
import { UserRole, InvoiceStatus, RequestStatus } from '../../../models';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatChipsModule
  ],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {
  private readonly router = inject(Router);
  private readonly userService = inject(UserService);
  private readonly invoiceService = inject(InvoiceService);
  private readonly changeRequestService = inject(ChangeRequestService);
  private readonly refundRequestService = inject(RefundRequestService);
  readonly currentUser = this.userService.currentUser;
  readonly isAdmin = computed(() => this.currentUser()?.role === UserRole.Admin);

  // Statistics computed from services
  readonly stats = computed(() => {
    const invoices = this.invoiceService.invoices();
    const changeRequests = this.changeRequestService.changeRequests();
    const refundRequests = this.refundRequestService.refundRequests();
    const users = this.userService.users();

    return {
      totalInvoices: invoices.length,
      paidInvoices: invoices.filter(inv => inv.status === InvoiceStatus.Paid).length,
      overdueInvoices: invoices.filter(inv => inv.status === InvoiceStatus.Overdue).length,
      pendingInvoices: invoices.filter(inv => inv.status === InvoiceStatus.Draft).length,

      totalChangeRequests: changeRequests.length,
      pendingChangeRequests: changeRequests.filter(req => req.status === RequestStatus.Pending).length,
      approvedChangeRequests: changeRequests.filter(req => req.status === RequestStatus.Approved).length,
      rejectedChangeRequests: changeRequests.filter(req => req.status === RequestStatus.Rejected).length,

      totalRefundRequests: refundRequests.length,
      pendingRefundRequests: refundRequests.filter(req => req.status === RequestStatus.Pending).length,
      approvedRefundRequests: refundRequests.filter(req => req.status === RequestStatus.Approved).length,
      completedRefundRequests: refundRequests.filter(req => req.status === RequestStatus.Approved).length, // Using Approved for now

      totalUsers: users.length,
      adminUsers: users.filter(user => user.role === UserRole.Admin).length,
      regularUsers: users.filter(user => user.role === UserRole.User).length
    };
  });

  readonly recentActivity = computed(() => {
    // Mock recent activity - in real app, this would come from an activity service
    const activity = [
      {
        id: '1',
        type: 'change_request',
        title: 'Nowe żądanie zmiany dla faktury INV-2024-001',
        user: 'Jan Kowalski',
        timestamp: new Date(Date.now() - 2 * 60 * 1000).toISOString(), // 2 minutes ago
        icon: 'edit',
        color: 'primary'
      },
      {
        id: '2',
        type: 'refund_request',
        title: 'Żądanie zwrotu dla faktury INV-2024-015 zostało zatwierdzone',
        user: 'Admin',
        timestamp: new Date(Date.now() - 15 * 60 * 1000).toISOString(), // 15 minutes ago
        icon: 'undo',
        color: 'accent'
      },
      {
        id: '3',
        type: 'user_registered',
        title: 'Nowy użytkownik zarejestrował się w systemie',
        user: 'Anna Nowak',
        timestamp: new Date(Date.now() - 45 * 60 * 1000).toISOString(), // 45 minutes ago
        icon: 'person_add',
        color: 'primary'
      },
      {
        id: '4',
        type: 'invoice_paid',
        title: 'Faktura INV-2024-012 została opłacona',
        user: 'System',
        timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(), // 2 hours ago
        icon: 'payment',
        color: ''
      }
    ];

    return activity;
  });

  readonly loading = computed(() =>
    this.invoiceService.loading() ||
    this.changeRequestService.loading() ||
    this.refundRequestService.loading() ||
    this.userService.loading()
  );

  ngOnInit() {
    // Redirect if not admin
    if (!this.isAdmin()) {
      this.router.navigate(['/invoices']);
      return;
    }

    // Load all data
    this.invoiceService.loadInvoices();
    this.changeRequestService.loadChangeRequests();
    this.refundRequestService.loadRefundRequests();
    this.userService.loadUsers();
  }

  formatRelativeTime(timestamp: string): string {
    const now = new Date();
    const time = new Date(timestamp);
    const diffInMinutes = Math.floor((now.getTime() - time.getTime()) / (1000 * 60));

    if (diffInMinutes < 1) return 'przed chwilą';
    if (diffInMinutes < 60) return `${diffInMinutes} min temu`;

    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) return `${diffInHours} godz. temu`;

    const diffInDays = Math.floor(diffInHours / 24);
    return `${diffInDays} dni temu`;
  }

  navigateToUsers() {
    this.router.navigate(['/admin/users']);
  }

  navigateToRequests() {
    this.router.navigate(['/admin/requests']);
  }

  navigateToInvoices() {
    this.router.navigate(['/invoices']);
  }

  navigateToChangeRequests() {
    this.router.navigate(['/change-requests']);
  }

  navigateToRefundRequests() {
    this.router.navigate(['/refund-requests']);
  }

  refreshData() {
    this.invoiceService.loadInvoices();
    this.changeRequestService.loadChangeRequests();
    this.refundRequestService.loadRefundRequests();
    this.userService.loadUsers();
  }
}
