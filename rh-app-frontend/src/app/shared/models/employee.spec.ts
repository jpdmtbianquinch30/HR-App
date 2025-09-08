import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from './employee';

describe('Employee Models', () => {
  it('should create an Employee instance', () => {
    const employee: Employee = {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      phone: '+33123456789',
      position: 'Développeur',
      salary: 45000,
      hireDate: new Date('2023-01-15'),
      departmentId: 1,
      userId: 1,
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    expect(employee).toBeTruthy();
    expect(employee.firstName).toBe('John');
    expect(employee.isActive).toBe(true);
  });

  it('should create a CreateEmployeeRequest instance', () => {
    const request: CreateEmployeeRequest = {
      firstName: 'Jane',
      lastName: 'Smith',
      email: 'jane.smith@example.com',
      phone: '+33987654321',
      position: 'Designer',
      salary: 40000,
      hireDate: new Date('2023-02-01'),
      departmentId: 2
    };

    expect(request).toBeTruthy();
    expect(request.salary).toBe(40000);
  });

  it('should create an UpdateEmployeeRequest instance', () => {
    const updateRequest: UpdateEmployeeRequest = {
      id: 1,
      salary: 50000,
      isActive: false
    };

    expect(updateRequest).toBeTruthy();
    expect(updateRequest.id).toBe(1);
    expect(updateRequest.isActive).toBe(false);
  });
});