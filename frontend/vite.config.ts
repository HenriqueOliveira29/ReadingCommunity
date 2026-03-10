import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3001,
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ''),
        secure: false  // Allow self-signed certificates in development
      }
    }
  }
})
