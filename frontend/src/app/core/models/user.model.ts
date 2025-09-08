export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: 'ADMIN' | 'MANAGER' | 'EMPLOYE';
  departmentId?: number;
  departmentName?: string;
  token?: string;
}