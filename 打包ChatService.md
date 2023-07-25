```shell
docker run --name chat -p 5006:80 --restart=always -d -v /root/apps/chat_docker/wwwroot:/app/wwwroot chat-service:0.0.6
```

```shell
docker build -t chat-service:0.0.6 .
```

