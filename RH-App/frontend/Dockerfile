FROM node:18-alpine AS build
WORKDIR /app
COPY frontend/package.json frontend/package-lock.json ./
RUN npm install
COPY frontend/ .
RUN npm run build -- --configuration production

FROM nginx:alpine
COPY --from=build /app/dist/frontend /usr/share/nginx/html
COPY frontend/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80