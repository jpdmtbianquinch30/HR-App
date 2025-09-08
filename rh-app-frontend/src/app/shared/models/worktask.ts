export interface WorkTask {
  id: number;
  title: string;
  description: string;
  status: 'PENDING' | 'IN_PROGRESS' | 'COMPLETED' | 'CANCELLED';
  priority: 'LOW' | 'MEDIUM' | 'HIGH' | 'URGENT';
  assignedToId: number;
  createdById: number;
  departmentId: number;
  dueDate: Date;
  completedDate?: Date;
  estimatedHours: number;
  actualHours?: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateWorkTaskRequest {
  title: string;
  description: string;
  priority: 'LOW' | 'MEDIUM' | 'HIGH' | 'URGENT';
  assignedToId: number;
  departmentId: number;
  dueDate: Date;
  estimatedHours: number;
}

export interface UpdateWorkTaskRequest extends Partial<CreateWorkTaskRequest> {
  id: number;
  status?: 'PENDING' | 'IN_PROGRESS' | 'COMPLETED' | 'CANCELLED';
  completedDate?: Date;
  actualHours?: number;
}