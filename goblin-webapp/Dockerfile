FROM node:18-alpine

WORKDIR /app

# Copy package.json and package-lock.json (if it exists)
# This ensures that we install the exact dependencies
COPY package*.json ./

# Install project dependencies
RUN npm install

# Copy the rest of the application code into the image
COPY . .

# Create React App typically runs on port 3000
EXPOSE 3000

# Command to start the development server
CMD ["npm", "start"]
