using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Entitities
{
    public interface IPlayerBullet
    {
        void CheckCollisionWithEnemyBullets(List<BulletBase> enemybullets);
    }
}
