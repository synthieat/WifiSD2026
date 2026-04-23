using SD.Core.Attributes;
using SD.Core.Repositories.Movies;
using SD.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Persistence.Repositories.Movies
{
    [MapServiceDependency(nameof(MediumTypeRepository))]
    public class MediumTypeRepository : BaseRepository, IMediumTypeRepository
    {
    }
}
