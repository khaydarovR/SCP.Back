﻿using MediatR;

namespace SCP.Application.Core.Safens.Commands
{
    public class CreateSafeCommand: IRequest<Unit>
    {
        public string Title { get; init; }
        public string? Description { get; init; }
        public string? ClearKey { get; init; }

    }
}
