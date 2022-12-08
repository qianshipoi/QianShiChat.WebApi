global using AutoMapper;
global using AutoMapper.QueryableExtensions;

global using EasyCaching.Core;
global using EasyCaching.Serialization.SystemTextJson.Configurations;

global using FluentValidation;

global using MediatR;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.StaticFiles;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using QianShiChat.Common.Extensions;
global using QianShiChat.Common.Helpers;
global using QianShiChat.Models;
global using QianShiChat.WebApi;
global using QianShiChat.WebApi.BackgroundHost;
global using QianShiChat.WebApi.Core.AutoDI;
global using QianShiChat.WebApi.Hubs;
global using QianShiChat.WebApi.Models;
global using QianShiChat.WebApi.Models.Entity;
global using QianShiChat.WebApi.Models.Requests;
global using QianShiChat.WebApi.Services;

global using Quartz;

global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using Yitter.IdGenerator;
global using SixLabors.ImageSharp.Web.DependencyInjection;