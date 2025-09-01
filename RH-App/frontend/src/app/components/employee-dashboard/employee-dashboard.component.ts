
@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrls: ['./employee-dashboard.component.css']
})
export class EmployeeDashboardComponent implements OnInit {
  tasks: any[] = [];
  loading = true;

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getMyTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading tasks', error);
        this.loading = false;
      }
    });
  }

  markAsCompleted(taskId: number): void {
    this.taskService.updateTaskStatus(taskId, 'TERMINE').subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (error) => {
        console.error('Error updating task', error);
      }
    });
  }
}