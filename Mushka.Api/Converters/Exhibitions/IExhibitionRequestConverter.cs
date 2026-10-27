using System;
using Mushka.Api.ClientModels.Exhibition;
using Mushka.Application.Exhibitions.Commands;

namespace Mushka.Api.Converters.Exhibitions;

public interface IExhibitionRequestConverter
{
    CreateExhibitionCommand ToCreateCommand(ExhibitionRequestModel request);

    UpdateExhibitionCommand ToUpdateCommand(Guid id, ExhibitionRequestModel request);
}
