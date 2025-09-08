import { WorkTask, CreateWorkTaskRequest, UpdateWorkTaskRequest } from './worktask';

describe('WorkTask Models', () => {
  it('should create a WorkTask instance', () => {
    const task: WorkTask = {
      id: 1,
      title: 'Développer nouvelle fonctionnalité',
      description: 'Implémenter le système d\'authentification',
      status: 'IN_PROGRESS',
      priority: 'HIGH',
      assignedToId: 1,
      createdById: 2,
      departmentId: 1,
      dueDate: new Date('2024-02-15'),
      completedDate: undefined,
      estimatedHours: 40,
      actualHours: 25,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    expect(task).toBeTruthy();
    expect(task.status).toBe('IN_PROGRESS');
    expect(task.priority).toBe('HIGH');
  });

  it('should create a CreateWorkTaskRequest instance', () => {
    const request: CreateWorkTaskRequest = {
      title: 'Tâche de test',
      description: 'Description de la tâche',
      priority: 'MEDIUM',
      assignedToId: 1,
      departmentId: 1,
      dueDate: new Date('2024-03-01'),
      estimatedHours: 8
    };

    expect(request).toBeTruthy();
    expect(request.estimatedHours).toBe(8);
  });

  it('should create an UpdateWorkTaskRequest instance', () => {
    const updateRequest: UpdateWorkTaskRequest = {
      id: 1,
      status: 'COMPLETED',
      completedDate: new Date(),
      actualHours: 10
    };

    expect(updateRequest).toBeTruthy();
    expect(updateRequest.status).toBe('COMPLETED');
  });
});