```shell
docker run --name chat -p 5006:80 --restart=always -d -v /root/apps/chat_docker/wwwroot:/app/wwwroot chat-service:0.0.6
```

```shell
docker build -t chat-service:0.0.6 .
```

```
docker run --name chat_new -p 7000:80 --restart=always -d -v /root/apps/chat_docker/wwwroot:/app/wwwroot -v /root/apps/chat_docker/appsettings.Local.json:/app/appsettings.Local.json chat-service:0.0.7

```

