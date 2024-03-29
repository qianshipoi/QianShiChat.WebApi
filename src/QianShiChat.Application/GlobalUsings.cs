﻿global using AutoMapper;
global using AutoMapper.QueryableExtensions;

global using CommunityToolkit.Diagnostics;

global using EasyCaching.Core;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.StaticFiles;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;

global using QianShiChat.Application.Common.Interfaces;
global using QianShiChat.Application.Common.IRepositories;
global using QianShiChat.Application.Contracts;
global using QianShiChat.Application.Contracts.Dtos;
global using QianShiChat.Application.Filters;
global using QianShiChat.Application.Hubs;
global using QianShiChat.Application.Managers;
global using QianShiChat.Application.Services;
global using QianShiChat.Application.Services.IServices;
global using QianShiChat.Domain;
global using QianShiChat.Domain.Core.AppOops;
global using QianShiChat.Domain.Core.AutoDI;
global using QianShiChat.Domain.Extensions;
global using QianShiChat.Domain.Helpers;
global using QianShiChat.Domain.Models;
global using QianShiChat.Domain.Options;
global using QianShiChat.Domain.Shared;

global using SixLabors.ImageSharp;
global using SixLabors.ImageSharp.Processing;

global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Runtime.CompilerServices;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Threading.Channels;

global using Yitter.IdGenerator;