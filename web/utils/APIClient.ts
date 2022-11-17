import axios from 'axios';

class APIClient {
    private static instance: APIClient;
    private static endpoint = 'http://localhost:3000/api/';
    
    private constructor() {}
    
    public static getInstance(): APIClient {
        if (!APIClient.instance) {
        APIClient.instance = new APIClient();
        }
    
        return APIClient.instance;
    }
    
    public async post<T>(path: string, data: any) {
        let config = {
            method: 'post',
            url: this.getURL(path),
            headers: {
              'Content-Type': 'application/x-www-form-urlencoded'
            },
            data: data
          };
          
        return axios<T>(config);
    }

    public async get(path: string) {
        return await axios.get(this.getURL(path));
    }

    private getURL(path: string) {
        return APIClient.endpoint + path;
    }

}

export default APIClient;