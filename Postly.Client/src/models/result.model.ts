export interface Result<T> {
   isSuccessful: boolean; 
  data?: T | null;         
  errorMessages?: string[];    
  statusCode?: number;
}
