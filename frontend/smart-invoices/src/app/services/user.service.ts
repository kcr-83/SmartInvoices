import { Injectable, signal, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable, of, delay, map } from 'rxjs';
import {
  User,
  CreateUserDto,
  UpdateUserDto,
  UserFilter,
  UserRole,
  AuthUser,
  LoginCredentials,
  LoginResponse,
  ApiResponse
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl = '/api/users';

  // Signals for state management
  private readonly _users = signal<User[]>([]);
  private readonly _currentUser = signal<AuthUser | null>(null);
  private readonly _loading = signal(false);

  // Public readonly signals
  readonly users = this._users.asReadonly();
  readonly currentUser = this._currentUser.asReadonly();
  readonly loading = this._loading.asReadonly();

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.loadMockData();
    this.loadCurrentUser();
  }

  // Authentication methods
  login(credentials: LoginCredentials): Observable<LoginResponse> {
    // Mock login - in real app would call API
    const mockUser: AuthUser = {
      id: 'user-1',
      email: credentials.email,
      firstName: 'Jan',
      lastName: 'Kowalski',
      role: UserRole.User,
      token: 'mock-jwt-token'
    };

    const response: LoginResponse = {
      user: mockUser,
      token: mockUser.token
    };

    return of(response).pipe(
      delay(1000),
      map(loginResponse => {
        this._currentUser.set(loginResponse.user);
        this.setStorageItem('currentUser', JSON.stringify(loginResponse.user));
        return loginResponse;
      })
    );
  }

  logout(): void {
    this._currentUser.set(null);
    this.removeStorageItem('currentUser');
  }

  // Check if user is authenticated
  isAuthenticated(): boolean {
    return this._currentUser() !== null;
  }

  // Check if user has admin role
  isAdmin(): boolean {
    const user = this._currentUser();
    return user?.role === UserRole.Admin || user?.role === UserRole.SuperAdmin;
  }

  // Load current user from localStorage
  private loadCurrentUser(): void {
    const stored = this.getStorageItem('currentUser');
    if (stored) {
      try {
        const user = JSON.parse(stored) as AuthUser;
        this._currentUser.set(user);
      } catch (error) {
        console.error('Error parsing stored user:', error);
        this.removeStorageItem('currentUser');
      }
    }
  }

  /**
   * Safely gets item from localStorage only in browser environment
   */
  private getStorageItem(key: string): string | null {
    if (!this.isBrowser()) {
      return null;
    }

    try {
      return localStorage.getItem(key);
    } catch (error) {
      console.error('Error accessing localStorage:', error);
      return null;
    }
  }

  /**
   * Safely sets item in localStorage only in browser environment
   */
  private setStorageItem(key: string, value: string): void {
    if (!this.isBrowser()) {
      return;
    }

    try {
      localStorage.setItem(key, value);
    } catch (error) {
      console.error('Error setting localStorage item:', error);
    }
  }

  /**
   * Safely removes item from localStorage only in browser environment
   */
  private removeStorageItem(key: string): void {
    if (!this.isBrowser()) {
      return;
    }

    try {
      localStorage.removeItem(key);
    } catch (error) {
      console.error('Error removing localStorage item:', error);
    }
  }

  /**
   * Checks if code is running in browser environment
   */
  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  // User management methods (admin functions)
  loadUsers(): Observable<User[]> {
    this._loading.set(true);

    return this.getMockUsers().pipe(
      delay(500),
      map(users => {
        this._users.set(users);
        this._loading.set(false);
        return users;
      })
    );
  }

  getUser(id: string): Observable<User | undefined> {
    return of(this._users().find(user => user.id === id)).pipe(
      delay(300)
    );
  }

  createUser(dto: CreateUserDto): Observable<User> {
    const newUser: User = {
      id: this.generateId(),
      email: dto.email,
      firstName: dto.firstName,
      lastName: dto.lastName,
      role: dto.role,
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    return of(newUser).pipe(
      delay(1000),
      map(user => {
        this._users.update(users => [...users, user]);
        return user;
      })
    );
  }

  updateUser(id: string, dto: UpdateUserDto): Observable<User> {
    return of(this._users().find(user => user.id === id)).pipe(
      delay(500),
      map(user => {
        if (!user) {
          throw new Error('User not found');
        }

        const updatedUser: User = {
          ...user,
          ...dto,
          updatedAt: new Date()
        };

        this._users.update(users =>
          users.map(u => u.id === id ? updatedUser : u)
        );

        return updatedUser;
      })
    );
  }

  deactivateUser(id: string): Observable<User> {
    return this.updateUser(id, { isActive: false });
  }

  activateUser(id: string): Observable<User> {
    return this.updateUser(id, { isActive: true });
  }

  // Filter users
  filterUsers(filter: UserFilter): Observable<User[]> {
    let result = this._users();

    if (filter.searchTerm) {
      const term = filter.searchTerm.toLowerCase();
      result = result.filter(user =>
        user.email.toLowerCase().includes(term) ||
        user.firstName.toLowerCase().includes(term) ||
        user.lastName.toLowerCase().includes(term)
      );
    }

    if (filter.role) {
      result = result.filter(user => user.role === filter.role);
    }

    if (filter.isActive !== undefined) {
      result = result.filter(user => user.isActive === filter.isActive);
    }

    return of(result).pipe(delay(300));
  }

  // Private helper methods
  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  private loadMockData(): void {
    this.loadUsers().subscribe();
  }

  private getMockUsers(): Observable<User[]> {
    const mockUsers: User[] = [
      {
        id: 'user-1',
        email: 'jan.kowalski@example.com',
        firstName: 'Jan',
        lastName: 'Kowalski',
        role: UserRole.User,
        isActive: true,
        createdAt: new Date('2025-01-01'),
        updatedAt: new Date('2025-01-01'),
        lastLoginAt: new Date('2025-05-29')
      },
      {
        id: 'user-2',
        email: 'anna.nowak@example.com',
        firstName: 'Anna',
        lastName: 'Nowak',
        role: UserRole.User,
        isActive: true,
        createdAt: new Date('2025-01-05'),
        updatedAt: new Date('2025-01-05'),
        lastLoginAt: new Date('2025-05-28')
      },
      {
        id: 'user-3',
        email: 'piotr.wisniewski@example.com',
        firstName: 'Piotr',
        lastName: 'Wiśniewski',
        role: UserRole.User,
        isActive: true,
        createdAt: new Date('2025-01-10'),
        updatedAt: new Date('2025-01-10'),
        lastLoginAt: new Date('2025-05-27')
      },
      {
        id: 'admin-1',
        email: 'admin@smartinvoices.com',
        firstName: 'Administrator',
        lastName: 'Systemu',
        role: UserRole.Admin,
        isActive: true,
        createdAt: new Date('2024-12-01'),
        updatedAt: new Date('2024-12-01'),
        lastLoginAt: new Date('2025-05-29')
      },
      {
        id: 'user-4',
        email: 'inactive.user@example.com',
        firstName: 'Nieaktywny',
        lastName: 'Użytkownik',
        role: UserRole.User,
        isActive: false,
        createdAt: new Date('2024-11-15'),
        updatedAt: new Date('2025-03-01')
      }
    ];

    return of(mockUsers);
  }
}
