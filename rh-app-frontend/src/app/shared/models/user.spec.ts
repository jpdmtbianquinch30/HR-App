import { User, LoginRequest, LoginResponse } from './user';

describe('User Models', () => {
  it('should create a User instance', () => {
    const user: User = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe',
      role: 'EMPLOYE',
      departmentId: 1,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    expect(user).toBeTruthy();
    expect(user.role).toBe('EMPLOYE');
  });

  it('should create a LoginRequest instance', () => {
    const loginRequest: LoginRequest = {
      email: 'test@example.com',
      password: 'password123'
    };

    expect(loginRequest).toBeTruthy();
    expect(loginRequest.email).toBe('test@example.com');
  });

  it('should create a LoginResponse instance', () => {
    const loginResponse: LoginResponse = {
      token: 'jwt-token',
      user: {
        id: 1,
        email: 'test@example.com',
        firstName: 'John',
        lastName: 'Doe',
        role: 'EMPLOYE',
        createdAt: new Date(),
        updatedAt: new Date()
      }
    };

    expect(loginResponse).toBeTruthy();
    expect(loginResponse.token).toBe('jwt-token');
  });
});