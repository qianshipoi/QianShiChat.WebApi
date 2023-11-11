FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY . .
RUN sed -i "s@http://\(deb\|security\).debian.org@https://mirrors.tencent.com@g" /etc/apt/sources.list
RUN apt-get update
RUN apt-get install -y locales locales-all
ENV LANG en_US.UTF-8
#RUN sed -i 's#http://archive.ubuntu.com/#http://mirrors.tuna.tsinghua.edu.cn/#' /etc/apt/sources.list;
#RUN apt-get update --fix-missing
#RUN apt-get install -y libgdiplus --fix-missing
#RUN apt-get clean 
#RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
EXPOSE 80
ENTRYPOINT ["dotnet", "QianShiChat.WebApi.dll"]