export interface Department {
  id: number;
  name: string;
  description: string;
  managerId?: number;
  budget: number;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  employeeCount?: number;
}

export interface CreateDepartmentRequest {
  name: string;
  description: string;
  managerId?: number;
  budget: number;
}

export interface UpdateDepartmentRequest extends Partial<CreateDepartmentRequest> {
  id: number;
  isActive?: boolean;
}