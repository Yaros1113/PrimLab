import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TasksService } from '../../services/tasks.service';
import { Task } from '../../models/task.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'title', 'status', 'createdDate', 'storeAddress', 'actions'];
  dataSource: MatTableDataSource<Task> = new MatTableDataSource<Task>([]);
  
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  search = '';
  sortField = 'CreatedDate';
  ascending = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private tasksService: TasksService,
    private snackBar: MatSnackBar
  ) {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.loadTasks();
  }

  loadTasks() {
    this.tasksService.getTasks(
      this.pageIndex + 1,
      this.pageSize,
      this.search,
      this.sortField,
      this.ascending
    ).subscribe({
      next: (data) => {
        this.dataSource.data = data.items;
        this.totalItems = data.totalCount;
      },
      error: (error) => console.error('Error loading tasks:', error)
    });
  }

  onPageChange(event: any) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadTasks();
  }

  onSortChange(event: any) {
    this.sortField = event.active;
    this.ascending = event.direction === 'asc';
    this.loadTasks();
  }

  applyFilter() {
    this.pageIndex = 0;
    this.loadTasks();
  }

  updateTaskStatus(task: Task, status: boolean) {
    this.tasksService.updateTaskStatus(task.id, status).subscribe({
      next: (updatedTask) => {
        // Обновление локальных данных
        const index = this.dataSource.data.findIndex(t => t.id === updatedTask.id);
        if (index !== -1) {
          this.dataSource.data[index] = updatedTask;
          this.dataSource._updateChangeSubscription();
        }
      },
      error: (error) => console.error('Error updating task status:', error)
    });
  }

  deleteTask(id: number) {
    if (confirm('Вы уверены, что хотите удалить эту задачу?')) {
      this.tasksService.deleteTask(id).subscribe({
        next: () => {
          // Удаление задачи из локального списка
          this.dataSource.data = this.dataSource.data.filter(task => task.id !== id);
          this.totalItems--;
          
          // Обновление отображения таблицы
          this.dataSource._updateChangeSubscription();
          this.dataSource.paginator = this.paginator;
          
          // Показать уведомление
          this.snackBar.open('Задача успешно удалена', 'Закрыть', {
            duration: 3000
          });
        },
        error: (error) => {
          console.error('Ошибка при удалении задачи:', error);
          this.snackBar.open('Не удалось удалить задачу', 'Закрыть', {
            duration: 3000,
            panelClass: ['error-snackbar']
          });
        }
      });
    }
  }
}