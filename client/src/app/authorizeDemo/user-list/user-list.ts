import { Component, Input } from '@angular/core';
import { UserService } from '../../shared/services/user-service';

@Component({
  selector: 'app-user-list',
  imports: [],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css',
})
export class UserList {
  users: any[] = [];
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getUserList();
  }
  getUserList() {
    this.userService.getUsers().subscribe({
      next: (response) => {
        console.log('Users:', response);
        this.users = response as any[];
      },
      error: (err) => {
        console.error('Error fetching users:', err);
      }
    });
  }
}
