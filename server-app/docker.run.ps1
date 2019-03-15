# change "-p port:80" to your docker host port, say "-p 8092:80"
# change http://host:port to your client app domain or ip to allow CORS, say http://localhost:8092
# e.g. docker run -d -p 8091:80 --env Cors__AllowedOrigins=http://192.168.0.108:8092 olmelabs/battleshipapi

docker run -d -p port:80 --env Cors__AllowedOrigins=http://host:port olmelabs/battleshipapi