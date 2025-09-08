export interface Task {
  id: number;
  title: string;
  description: string;
  status: 'PENDING' | 'IN_PROGRESS' | 'COMPLETED';
  priority: 'LOW' | 'MEDIUM' | 'HIGH';
  assignedToId: number;
  assignedToName: string;
  createdById: number;
  createdByName: string;
  createdAt: Date;
  dueDate: Date;
  completedAt?: Date;
  departmentId: number;
  departmentName: string;
}