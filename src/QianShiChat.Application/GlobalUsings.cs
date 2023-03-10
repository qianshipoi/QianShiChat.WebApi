global using AutoMapper;
global using AutoMapper.QueryableExtensions;

global using EasyCaching.Core;

global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;

global using QianShiChat.Application.Contracts;
global using QianShiChat.Application.Hubs;
global using QianShiChat.Common.Extensions;
global using QianShiChat.Domain;
global using QianShiChat.Domain.Core.AppOops;
global using QianShiChat.Domain.Core.AutoDI;
global using QianShiChat.Domain.Models;
global using QianShiChat.Domain.Options;
global using QianShiChat.Domain.Shared;

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;

global using Yitter.IdGenerator;