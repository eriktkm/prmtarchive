(function () {
    var categories = [
        {
            title: 'MINHA CONTA', icon: '◆',
            items: [
                ['PERFIL', 'perfil'],
                ['EDITAR PERFIL', 'editar-perfil'],
                ['NOTIFICAÇÕES', 'notificacoes'],
                ['CONFIGURAÇÕES', 'configuracoes']
            ]
        },
        {
            title: 'COMUNIDADE', icon: '◆',
            items: [
                ['COMUNIDADE', 'comunidade'],
                ['CRIAR COMUNIDADE', 'criar-comunidade'],
                ['NOVA PUBLICAÇÃO', 'criar-publicacao'],
                ['POST', 'post'],
            ]
        },
        {
            title: 'ÁREA DO ARTISTA', icon: '◆',
            items: [
                ['DASHBOARD', 'dashboard-artista'],
                ['GERENCIAR MÚSICAS', 'gerenciar-musicas'],
                ['CRIAR LANÇAMENTO', 'criar-lancamento'],
                ['ESTATÍSTICAS', 'estatisticas-artista']
            ]
        },
        {
            title: 'ADMINISTRAÇÃO', icon: '◆',
            items: [
                ['DASHBOARD ADMIN', 'dashboard-administrativo'],
                ['GERENCIAR USUÁRIOS', 'gerenciar-usuarios'],
                ['GERENCIAR CONTEÚDO', 'gerenciar-conteudo'],
                ['DENÚNCIAS', 'denuncias'],
                ['GERENCIAR COMUNIDADES', 'gerenciar-comunidades']
            ]
        }
    ];

    var header = document.querySelector('.header');
    if (!header || document.querySelector('.site-menu')) return;

    var style = document.createElement('style');
    style.textContent = `
        .menu-button {
            position: relative;
            display: inline-flex;
            align-items: center;
            gap: 9px;
            margin-left: 14px;
            padding: 11px 16px;
            border: 2px solid #000;
            background: #91a7e2;
            color: #000;
            font-family: 'Press Start 2P', monospace;
            font-size: 9px;
            line-height: 1;
            cursor: pointer;
            box-shadow: 4px 4px 0 #000;
            transition: .12s ease;
        }
        .menu-button::before { content: '☰'; font-size: 14px; }
        .menu-button:hover { background: #dae3ec; transform: translate(-2px,-2px); box-shadow: 6px 6px 0 #000; }
        .menu-button:active { transform: translate(2px,2px); box-shadow: 2px 2px 0 #000; }

        .site-menu {
            position: fixed;
            inset: 0;
            z-index: 99999;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 24px;
            background: rgba(0,0,0,.78);
            backdrop-filter: blur(5px);
            opacity: 0;
            visibility: hidden;
            pointer-events: none;
            transition: opacity .18s ease, visibility .18s ease;
        }
        .site-menu.open { opacity: 1; visibility: visible; pointer-events: auto; }
        body.menu-open { overflow: hidden; }

        .site-menu-box {
            position: relative;
            width: min(1050px, 100%);
            max-height: min(850px, 92vh);
            overflow: auto;
            padding: 24px;
            background: #dae3ec;
            color: #000;
            border: 4px solid #000;
            box-shadow: 12px 12px 0 #7085ca, inset 0 0 0 2px #91a7e2;
            transform: translateY(18px) scale(.98);
            transition: transform .18s ease;
        }
        .site-menu.open .site-menu-box { transform: translateY(0) scale(1); }
        .site-menu-box::-webkit-scrollbar { width: 10px; }
        .site-menu-box::-webkit-scrollbar-track { background: #91a7e2; border-left: 2px solid #000; }
        .site-menu-box::-webkit-scrollbar-thumb { background: #000; }

        .site-menu-head {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 20px;
            padding: 4px 0 18px;
            margin-bottom: 18px;
            border-bottom: 3px solid #000;
        }
        .site-menu-kicker {
            display: block;
            margin-bottom: 9px;
            font-family: 'VT323', monospace;
            font-size: 18px;
            letter-spacing: 2px;
        }
        .site-menu-head h2 {
            margin: 0;
            font-family: 'Press Start 2P', monospace;
            font-size: clamp(14px, 2vw, 21px);
            line-height: 1.5;
        }
        .site-menu-status {
            margin-top: 8px;
            font-family: 'VT323', monospace;
            font-size: 17px;
        }
        .site-menu-close {
            flex: 0 0 auto;
            width: 46px;
            height: 46px;
            border: 3px solid #000;
            background: #7085ca;
            color: #000;
            font-family: 'Press Start 2P', monospace;
            font-size: 18px;
            cursor: pointer;
            box-shadow: 4px 4px 0 #000;
        }
        .site-menu-close:hover { background: #fff; }

        .site-menu-categories {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 14px;
        }
        .site-menu-category {
            border: 2px solid #000;
            background: rgba(255,255,255,.45);
            box-shadow: 4px 4px 0 #91a7e2;
        }
        .site-menu-category-button {
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            padding: 14px 15px;
            border: 0;
            border-bottom: 2px solid #000;
            background: #91a7e2;
            color: #000;
            font-family: 'Press Start 2P', monospace;
            font-size: 9px;
            text-align: left;
            cursor: pointer;
        }
        .site-menu-category-button:hover { background: #7085ca; }
        .site-menu-category-button .arrow {
            font-size: 13px;
            transition: transform .15s ease;
        }
        .site-menu-category.open .arrow { transform: rotate(90deg); }
        .site-menu-list {
            display: none;
            padding: 8px;
        }
        .site-menu-category.open .site-menu-list { display: block; }
        .site-menu-link {
            position: relative;
            display: flex;
            align-items: center;
            min-height: 38px;
            margin-top: 6px;
            padding: 8px 9px 8px 25px;
            text-decoration: none;
            color: #000;
            background: #fff;
            border: 2px solid #000;
            font-family: 'VT323', monospace;
            font-size: 18px;
            line-height: 1;
            transition: .1s ease;
        }
        .site-menu-link::before { content: '>'; position: absolute; left: 9px; font-family: 'Press Start 2P', monospace; font-size: 8px; }
        .site-menu-link:hover { background: #7085ca; transform: translate(3px,-2px); box-shadow: 3px 3px 0 #000; }
        .site-menu-link.active { background: #7085ca; }

        @media (max-width: 700px) {
            .site-menu { padding: 12px; align-items: flex-start; }
            .site-menu-box { padding: 15px; max-height: 94vh; }
            .site-menu-categories { grid-template-columns: 1fr; }
            .menu-button { font-size: 8px; padding: 10px 12px; }
            .site-menu-head h2 { font-size: 12px; }
        }
    `;
    document.head.appendChild(style);

    var button = document.createElement('button');
    button.className = 'menu-button';
    button.type = 'button';
    button.textContent = 'MENU';
    button.setAttribute('aria-label', 'Abrir menu');

    var overlay = document.createElement('div');
    overlay.className = 'site-menu';
    overlay.innerHTML = `
        <div class="site-menu-box">
            <div class="site-menu-head">
                <div>
                    <span class="site-menu-kicker">PRIMATA ARCHIVE // NAVEGAÇÃO</span>
                    <h2>MENU PRINCIPAL</h2>
                    <div class="site-menu-status">STATUS: ONLINE // ESCOLHA UMA CATEGORIA_</div>
                </div>
                <button class="site-menu-close" type="button" aria-label="Fechar menu">×</button>
            </div>
            <div class="site-menu-categories"></div>
        </div>
    `;

    var grid = overlay.querySelector('.site-menu-categories');
    var currentPath = window.location.pathname;

    categories.forEach(function (category, index) {
        var block = document.createElement('section');
        block.className = 'site-menu-category';

        var categoryButton = document.createElement('button');
        categoryButton.className = 'site-menu-category-button';
        categoryButton.type = 'button';
        categoryButton.innerHTML = '<span><span style="margin-right:8px">' + category.icon + '</span>' + category.title + '</span><span class="arrow">▶</span>';

        var list = document.createElement('div');
        list.className = 'site-menu-list';

        category.items.forEach(function (item) {
            var link = document.createElement('a');
            link.className = 'site-menu-link';
            link.href = '../' + item[1] + '/index.html';
            link.textContent = item[0];
            if (currentPath.indexOf('/' + item[1] + '/') !== -1) link.classList.add('active');
            link.addEventListener('click', closeMenu);
            list.appendChild(link);
        });

        categoryButton.addEventListener('click', function () {
            block.classList.toggle('open');
        });

        block.appendChild(categoryButton);
        block.appendChild(list);
        grid.appendChild(block);
    });

    var nav = header.querySelector('.nav');
    if (nav && !nav.querySelector('.nav-search')) {
        var searchLink = document.createElement('a');
        searchLink.className = 'nav-search';
        searchLink.href = '../resultados-pesquisa/index.html';
        searchLink.textContent = '⌕';
        searchLink.setAttribute('aria-label', 'Pesquisar');
        searchLink.title = 'Pesquisar';
        nav.appendChild(searchLink);
    }

    header.appendChild(button);
    document.body.appendChild(overlay);

    function openMenu() {
        overlay.classList.add('open');
        document.body.classList.add('menu-open');
    }
    function closeMenu() {
        overlay.classList.remove('open');
        document.body.classList.remove('menu-open');
    }

    button.addEventListener('click', openMenu);
    overlay.querySelector('.site-menu-close').addEventListener('click', closeMenu);
    overlay.addEventListener('click', function (event) {
        if (event.target === overlay) closeMenu();
    });
    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') closeMenu();
    });
})();
