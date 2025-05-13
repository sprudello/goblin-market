# Stage 1: Development server (uses react-scripts start)
FROM node:18-alpine
WORKDIR /app

# Copy package.json and package-lock.json (or yarn.lock)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
# This will be overridden by the volume mount in docker-compose for development
COPY . .

# Expose the port React app runs on
EXPOSE 3000

# Start the development server
CMD ["npm", "start"]
