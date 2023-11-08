global using CommunityToolkit.Diagnostics;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using QianShiChat.Application.Common.Interfaces;
global using QianShiChat.Application.Common.IRepositories;
global using QianShiChat.Domain;
global using QianShiChat.Domain.Core.AutoDI;
global using QianShiChat.Domain.Core.Interceptors;
global using QianShiChat.Domain.Extensions;
global using QianShiChat.Domain.Models;
global using QianShiChat.Domain.Shared;
global using QianShiChat.Infrastructure.Data;
global using QianShiChat.Infrastructure.Data.Interceptors;

global using System.Linq.Expressions;
