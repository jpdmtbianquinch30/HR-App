export interface Employee {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: 'ADMIN' | 'MANAGER' | 'EMPLOYE';
  departmentId: number;
  departmentName: string;
  hireDate: Date;
  position: string;
  phoneNumber?: string;
  isActive: boolean;
}