import { Department, CreateDepartmentRequest, UpdateDepartmentRequest } from './department';

describe('Department Models', () => {
  it('should create a Department instance', () => {
    const department: Department = {
      id: 1,
      name: 'Développement',
      description: 'Équipe de développement logiciel',
      managerId: 1,
      budget: 500000,
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date(),
      employeeCount: 5
    };

    expect(department).toBeTruthy();
    expect(department.name).toBe('Développement');
    expect(department.isActive).toBe(true);
  });

  it('should create a CreateDepartmentRequest instance', () => {
    const request: CreateDepartmentRequest = {
      name: 'Marketing',
      description: 'Équipe marketing et communication',
      managerId: 2,
      budget: 300000
    };

    expect(request).toBeTruthy();
    expect(request.budget).toBe(300000);
  });

  it('should create an UpdateDepartmentRequest instance', () => {
    const updateRequest: UpdateDepartmentRequest = {
      id: 1,
      budget: 600000,
      isActive: true
    };

    expect(updateRequest).toBeTruthy();
    expect(updateRequest.id).toBe(1);
  });
});