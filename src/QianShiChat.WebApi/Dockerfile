FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY . .
#RUN mv /etc/apt/sources.list /etc/apt/sources.list.bak 
#RUN mv sources.list /etc/apt/ 
RUN sed -i 's#http://archive.ubuntu.com/#http://mirrors.tuna.tsinghua.edu.cn/#' /etc/apt/sources.list;
RUN apt-get update --fix-missing
RUN apt-get install -y libgdiplus --fix-missing
RUN apt-get clean 
RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
EXPOSE 80
ENTRYPOINT ["dotnet", "QianShiChat.WebApi.dll"]